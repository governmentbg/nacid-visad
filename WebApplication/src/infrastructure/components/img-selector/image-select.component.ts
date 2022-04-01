import { Component, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { AttachedFile } from 'src/infrastructure/models/attached-file.model';
import { FileUploadService } from 'src/infrastructure/services/file-upload.service';

@Component({
  selector: 'app-image-select',
  templateUrl: './image-select.component.html',
  styleUrls: ['./image-select.styles.css'],
  providers: [
    FileUploadService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ImageSelectComponent),
      multi: true
    }
  ]
})
export class ImageSelectComponent {
  imgSrc: SafeResourceUrl = "assets/unknown.jpg";
  imageFormats: Array<string> = ['image/png', 'image/jpg', 'image/jpeg', 'image/gif', 'image/tiff', 'image/bpg'];

  @Input() disabled = false;

  @Input('image')
  set modelSetter(value: AttachedFile) {
    if (value) {
      this.model = value;
      this.loadImage(this.model);
    }
  }

  model: AttachedFile | null;
  fileUrl: string;

  private fileStorageUrl: any;

  constructor(
    private configuration: Configuration,
    private service: FileUploadService,
    private sanitizer: DomSanitizer,
    private toastrService: ToastrService
  ) {
    this.fileStorageUrl = `${this.configuration.restUrl}/FilesStorage`;
  }

  propagateChange = (_: any) => { };
  propagateTouched = () => { };

  writeValue(obj: any): void {
    this.model = obj;
    this.setFileUrl();
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.propagateTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  changeImage(event: any): void {
    if (this.disabled) {
      return;
    }
    const target = event.target || event.srcElement;
    const files = target.files;
    if (files.length > 0) {
      if (!this.imageFormats.includes(files[0].type)) {
        this.toastrService.error('Невалиден формат на снимка');
        return;
      }

      this.service.uploadFile(this.fileStorageUrl, files[0])
        .subscribe((result) => {
          this.markAsUploaded(files[0], result);
        });
    }
  }

  private setFileUrl(): void {
    if (!this.model) {
      return;
    }

    this.fileUrl = `${this.fileStorageUrl}?key=${this.model.key}&fileName=${this.model.name}&dbId=${this.model.dbId}`;
  }

  private markAsUploaded(file: File, additionalInfo: AttachedFile): void {
    if (!this.model) {
      this.model = new AttachedFile();
    }

    this.model.name = file.name;
    this.model.mimeType = file.type;
    this.model.size = file.size;
    this.model.key = additionalInfo.key || (additionalInfo as any).fileKey;
    this.model.hash = additionalInfo.hash;
    this.model.dbId = additionalInfo.dbId;

    this.loadImage(this.model);
    this.propagateChange(this.model);
  }

  private loadImage(imgFile: AttachedFile): void {
    this.service.getBase64ImageUrl(imgFile.key, imgFile.dbId).subscribe((bas64ImgUrl: string) => {
      this.imgSrc = this.sanitizer.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${bas64ImgUrl}`);
    })
  }
}
