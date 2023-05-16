import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    constructor(public layoutService: LayoutService) { }

    ngOnInit() {
        this.model = [
            {
                label: 'Домашня сторінка',
                items: [
                    {
                        label: 'Авторизація',
                        icon: 'pi pi-fw pi-user',
                        items: [
                            {
                                label: 'Увійти',
                                icon: 'pi pi-fw pi-sign-in',
                                routerLink: ['/auth/login']
                            },
                        ]
                    },
                ]
            },
            {
                label: 'Астрономічні явища',
                icon: 'pi pi-fw pi-briefcase',
                items: [
                    {
                        label: 'Усі події',
                        icon: 'pi pi-fw pi-calendar',
                        routerLink: ['/event/all']
                    },
                    {
                        label: 'Найближчі події',
                        icon: 'pi pi-fw pi-calendar',
                        routerLink: ['/event/collection']
                    },
                ]
            }
        ];
    }
}