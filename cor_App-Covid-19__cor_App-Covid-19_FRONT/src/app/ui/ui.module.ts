import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSortModule } from '@angular/material/sort';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { PipesModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { AvatarComponent } from './avatar/avatar.component';
import { BackBtnComponent } from './back-btn/back-btn.component';
import { BreadcrumbsComponent } from './breadcrumbs/breadcrumbs.component';
import { HeaderNotificationsComponent } from './header-notifications/header-notifications.component';
import { HeaderComponent } from './header/header.component';
import { LoadingComponent } from './loading/loading.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { StepperComponent } from './stepper/stepper.component';
import { TableComponent } from './table/table.component';
import { TestResultComponent } from './test-result/test-result.component';

const exportComponents = [
  HeaderComponent,
  SidenavComponent,
  TableComponent,
  TestResultComponent,
  LoadingComponent,
  StepperComponent,
  BackBtnComponent
];
@NgModule({
  declarations: [
    ...exportComponents,
    AvatarComponent,
    BreadcrumbsComponent,
    HeaderNotificationsComponent,
    StepperComponent
  ],
  imports: [
    CommonModule,
    I18NextModule,
    RouterModule,
    MatSidenavModule,
    MatIconModule,
    MatTableModule,
    MatProgressSpinnerModule,
    MatStepperModule,
    PipesModule,
    MatButtonModule,
    MatSortModule,
    MatMenuModule,
    MatDialogModule,
    MatSelectModule,
    MatPaginatorModule
  ],
  exports: exportComponents
})
export class UiModule {}
