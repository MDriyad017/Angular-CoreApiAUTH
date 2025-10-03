import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { authGuard } from './shared/auth.guard';
import { AdminOnlyComponent } from './authorizetion/admin-only/admin-only.component';
import { AdminOrFactoryManagerComponent } from './authorizetion/admin-or-factory-manager/admin-or-factory-manager.component';
import { AdminOrShopManagerComponent } from './authorizetion/admin-or-shop-manager/admin-or-shop-manager.component';
import { ApplyForMaternityLeaveComponent } from './authorizetion/apply-for-maternity-leave/apply-for-maternity-leave.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { claimReq } from './shared/utils/claimReq-utils';
import { ProductInsertComponent } from './Insert/product-insert/product-insert.component';

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
  {
    path: '', component: MainLayoutComponent, canActivate: [authGuard],
    canActivateChild: [authGuard],
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { 
        path: 'admin-only', component: AdminOnlyComponent,
        data: { claimReq: claimReq.adminOnly }
      },
      { 
        path: 'admin-or-factory-manager', component: AdminOrFactoryManagerComponent,
        data: { claimReq: claimReq.adminOrFactoryManager }
      },
      { 
        path: 'admin-or-shop-manager', component: AdminOrShopManagerComponent,
        data: { claimReq: claimReq.hasLocationId }
      },
      { 
        path: 'apply-for-maternity-leave', component: ApplyForMaternityLeaveComponent,
        data: { claimReq: claimReq.femaleAndAdmin }
      },
      { 
        path: 'product-insert', component: ProductInsertComponent
      },
      { path: 'forbidden', component: ForbiddenComponent},
    ]
  },

];

