import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login.component';
import { UserActivationComponent } from './components/user-create/user-activation/user-activation.component';
import { UserCreateComponent } from './components/user-create/user-create.component';
import { UserEditComponent } from './components/user-edit/user-edit.component';
import { UserForgottenPasswordComponent } from './components/user-forgotten-password/user-forgotten-password.component';
import { UserPasswordRecoveryComponent } from './components/user-password-recovery/user-password-recovery.component';
import { UserSearchComponent } from './components/user-search/user-search/user-search.component';
import { UserTokenGuard } from './services/user-token.guard';

const routes: Routes = [
	{
		path: 'login',
		component: LoginComponent
	},
	{
		path: 'forgottenPassword',
		component: UserForgottenPasswordComponent
	},
	{
		path: 'user/create',
		component: UserCreateComponent
	},
	{
		path: 'user/search',
		component: UserSearchComponent
	},
	{
		path: 'user/activation',
		component: UserActivationComponent,
		canActivate: [UserTokenGuard]
	},
	{
		path: 'passwordRecovery',
		component: UserPasswordRecoveryComponent,
		canActivate: [UserTokenGuard]
	},
	{
		path: 'user/search/:id',
		component: UserEditComponent
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class UserRoutingModule { }
