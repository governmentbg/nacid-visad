import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { IMenuItem } from 'src/infrastructure/interfaces/IMenuItem';
import { RoleService } from 'src/infrastructure/services/role.service';

@Component({
  selector: 'app-menu',
  templateUrl: './app-menu.component.html',
  styleUrls: ['./app-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppMenuComponent implements OnInit {
  @Input() menuItems: IMenuItem[] = [];
  filePath: string;
  isCollapsed = false;

  constructor(
    private roleService: RoleService
  ) { }

  ngOnInit(): void {
    if (this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR)) {
      this.filePath = 'assets/documents/VisaD Admin Help.pdf'
    } else if (this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER, UserRoleAliases.RESULT_SIGNER_USER)) {
      this.filePath = 'assets/documents/VisaD MON Help.pdf'
    } else if (this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER)) {
      this.filePath = 'assets/documents/VisaD HS Help.pdf'
    } else if (this.roleService.hasRole(UserRoleAliases.CONTROL_USER)) {
      this.filePath = 'assets/documents/VisaD MVR Help.pdf'
    } else {
      this.filePath = 'assets/documents/VisaD HS Help.pdf'
    }
  }
}
