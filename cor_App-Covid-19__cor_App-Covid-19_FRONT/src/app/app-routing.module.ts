import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { EmployeeModule } from './employee/employee.module';
import { EmployeeComponent } from './employee/employee/employee.component';
import { EmployeesListComponent } from './employee/list/employees-list/employees-list.component';
import { NotificationsComponent } from './notifications/notifications/notifications.component';
import { NewUserComponent } from './outsourcing/new-user/new-user.component';
import { OutsourcingEmployeeComponent } from './outsourcing/outsourcing-employee/outsourcing-employee.component';
import { OutsourcingListComponent } from './outsourcing/outsourcing-list/outsourcing-list.component';
import { OutsourcingModule } from './outsourcing/outsourcing.module';
import { RouteRedirectGuard } from './route-redirect.guard';

const routes: Routes = [
  { path: '', redirectTo: 'list', pathMatch: 'full' },
  {
    path: 'list',
    component: EmployeesListComponent,
    data: {
      id: 'list',
      title: 'BREADCRUMBS.MEDICAL_SERVICES',
      roles: ['PRL', 'ServicioMedico', 'RRHHCent', 'RRHHDesc']
    },
    canActivate: [RouteRedirectGuard, MsalGuard]
  },
  {
    path: 'employee/:id',
    component: EmployeeComponent,
    data: {
      parent: 'list',
      title: '::Employee',
      roles: ['PRL', 'ServicioMedico', 'RRHHCent', 'RRHHDesc']
    },
    canActivate: [RouteRedirectGuard, MsalGuard]
  },
  {
    path: 'notifications',
    component: NotificationsComponent,
    data: {
      title: 'BREADCRUMBS.NOTIFICATIONS',
      roles: ['Comunicacion', 'ServicioMedico']
    },
    canActivate: [RouteRedirectGuard, MsalGuard]
  },
  {
    path: 'outsourcing',
    data: {
      roles: ['GestorContratas']
    },
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      {
        path: 'list',
        component: OutsourcingListComponent,
        data: {
          title: 'BREADCRUMBS.OUTSOURCING_LIST'
        }
      },
      {
        path: 'new-user',
        component: NewUserComponent,
        data: {
          title: 'BREADCRUMBS.NEW_USER'
        }
      },
      {
        path: 'employee/:id',
        component: OutsourcingEmployeeComponent,
        data: {
          title: '::Outsourcing'
        }
      }
    ],
    canActivate: [RouteRedirectGuard, MsalGuard]
  },
  { path: '**', redirectTo: 'list' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes), EmployeeModule, OutsourcingModule],
  exports: [RouterModule]
})
export class AppRoutingModule {}
