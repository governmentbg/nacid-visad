import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { IMenuItem } from 'src/infrastructure/interfaces/IMenuItem';
import { UserLoginEventEnum } from 'src/modules/user/enums/user-login-event.enum';
import { UserLoginService } from 'src/modules/user/services/user-login.service';
import { MenuItemsService } from './app-menu/services/menu-items.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isLoggedIn = false;
  menuItems: IMenuItem[] = [];

  constructor(
    private userService: UserLoginService,
    private menuItemsService: MenuItemsService,
    private translate: TranslateService
  ) {
    this.translate.setDefaultLang('bg');
    this.translate.use('bg');
  }

  ngOnInit(): void {
    this.isLoggedIn = this.userService.isLogged;
    this.menuItems = this.menuItemsService.getMainMenuItems(this.isLoggedIn);

    this.userService.subscribe((value: { event: UserLoginEventEnum }) => {
      this.isLoggedIn = value.event === UserLoginEventEnum.login;
      this.menuItems = this.menuItemsService.getMainMenuItems(this.isLoggedIn);
    });
  }
}
