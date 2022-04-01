export interface IMenuItem {
	title: string;
	routerLink?: string;
	icon?: string;
	isVisible: boolean;
	children?: IMenuItem[];
}
