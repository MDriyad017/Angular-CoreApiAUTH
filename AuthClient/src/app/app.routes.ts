import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { authGuard } from './shared/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: UserComponent,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' }, // 👈 default route
      { path: 'signup', component: RegistrationComponent },
      { path: 'login', component: LoginComponent }
    ]
  },
  { path: 'dashboard', component:DashboardComponent, canActivate:[authGuard] }
];

