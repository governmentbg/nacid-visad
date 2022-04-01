import { Injectable } from '@angular/core';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { IMenuItem } from 'src/infrastructure/interfaces/IMenuItem';
import { RoleService } from 'src/infrastructure/services/role.service';

@Injectable({
  providedIn: 'root'
})
export class MenuItemsService {
  constructor(
    private roleService: RoleService
  ) { }

  getMainMenuItems(isLoggedInUser: boolean): IMenuItem[] {
    if (!isLoggedInUser) {
      return [];
    }

    const isAdministrator = this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR);
    const isUniversityUser = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);

    return [
      { title: 'Заявления', icon: 'file-earmark-fill', routerLink: '/application/search', isVisible: !isAdministrator },
      {
        title: 'Справки', icon: 'bar-chart-fill', routerLink: '/reports', isVisible: !isAdministrator
      },
      {
        title: 'Заявления', icon: 'file-earmark-fill', isVisible: isAdministrator, children: [
          { title: 'Преглед', icon: 'file-text-fill', routerLink: '/application/search', isVisible: isAdministrator },
          { title: 'Справки', icon: 'bar-chart-fill', routerLink: '/reports', isVisible: isAdministrator }
        ]
      },
      { title: 'Кандидати', icon: 'file-earmark-person-fill', routerLink: '/candidate/search', isVisible: !isUniversityUser },
      {
        title: 'Администриране', icon: 'wrench', isVisible: isAdministrator, children: [
          { title: 'Потребители', icon: 'people-fill', routerLink: '/user/search', isVisible: isAdministrator },
          { title: 'Номенклатури', icon: 'collection-fill', routerLink: '/nomenclature', isVisible: isAdministrator },
        ]
      }
    ];
  }
}
