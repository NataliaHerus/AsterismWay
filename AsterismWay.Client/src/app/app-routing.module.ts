import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { AppLayoutComponent } from "./layout/app.layout.component";
import { LoginComponent } from './demo/components/auth/login/login.component';

@NgModule({
    imports: [
        RouterModule.forRoot([
            {   
                path: '', component: AppLayoutComponent,
                children: [
                    { path: 'event', loadChildren: () => import('./demo/components/event/event.module').then(m => m.EventModule)},
                    { path: 'profile', loadChildren: () => import('./demo/components/profile/profile.module').then(m => m.ProfileModule)}
                ]
            },
            { path: 'auth', loadChildren: () => import('./demo/components/auth/auth.module').then(m => m.AuthModule) },
            { path: 'notfound', component: NotfoundComponent },
            { path: '**', redirectTo: '/notfound' },
        ], { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
