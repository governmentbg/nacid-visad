import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { AttachedFile } from 'src/infrastructure/models/attached-file.model';
import { FileUploadService } from 'src/infrastructure/services/file-upload.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],
  providers: [
    FileUploadService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FileUploadComponent),
      multi: true
    }
  ]
})
export class FileUploadComponent implements ControlValueAccessor {
  constructor(
    private configuration: Configuration,
    private service: FileUploadService,
  ) {
    this.fileStorageUrl = `${this.configuration.restUrl}/FilesStorage`;
  }

  @Input() disabled = false;

  @Input() showFileUrl = false;

  @Input() required = false;

  model: AttachedFile | null;
  fileUrl: string;

  private fileStorageUrl: any;

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

  uploadFile(event: any): void {
    if (this.disabled) {
      return;
    }

    const target = event.target || event.srcElement;
    const files = target.files;
    if (files.length > 0) {
      this.service.uploadFile(this.fileStorageUrl, files[0])
        .subscribe((result) => {
          this.markAsUploaded(files[0], result);
        });
    }
  }

  deleteFile(): void {
    if (this.disabled) {
      return;
    }

    this.model = null;

    this.setFileUrl();
    this.propagateChange(this.model);
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

    this.setFileUrl();
    this.propagateChange(this.model);
  }

  private setFileUrl(): void {
    if (!this.model) {
      return;
    }

    this.fileUrl = `${this.fileStorageUrl}?key=${this.model.key}&fileName=${this.model.name}&dbId=${this.model.dbId}`;
  }

}
