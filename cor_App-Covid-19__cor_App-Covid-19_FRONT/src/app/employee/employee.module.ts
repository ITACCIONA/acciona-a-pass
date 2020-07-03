import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { RouterModule } from '@angular/router';
import { PipesModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../ui/ui.module';
import { ChangeStatusSimpleComponent } from './change-status-simple/change-status-simple.component';
import { EmployeeAddTestModalComponent } from './employee-add-test-modal/employee-add-test-modal.component';
import { EmployeeAddTestModalService } from './employee-add-test-modal/employee-add-test-modal.service';
import { EmployeeChangeStatusModalComponent } from './employee-change-status-modal/employee-change-status-modal.component';
import { EmployeeMedicalDataComponent } from './employee-medical-data/employee-medical-data.component';
import { FeverModalComponent } from './employee-medical-data/fever-modal/fever-modal.component';
import { FeverRecordsComponent } from './employee-medical-data/fever-records/fever-records.component';
import { MonitoringComponent } from './employee-medical-data/monitoring/monitoring.component';
import { PassportWarningComponent } from './employee-medical-data/passport-warning/passport-warning.component';
import { PassportComponent } from './employee-medical-data/passport/passport.component';
import { ResponsibleStatementComponent } from './employee-medical-data/responsible-statement/responsible-statement.component';
import { RiskFactorsComponent } from './employee-medical-data/risk-factors/risk-factors.component';
import { TestsComponent } from './employee-medical-data/tests/tests.component';
import { EmployeeService } from './employee.service';
import { EmployeePersonalDataComponent } from './employee/employee-personal-data/employee-personal-data.component';
import { EmployeeComponent } from './employee/employee.component';
import { EmployeesComplexFiltersComponent } from './list/employees-complex-filters/employees-complex-filters.component';
import { EmployeesFiltersComponent } from './list/employees-filters/employees-filters.component';
import { EmployeesListComponent } from './list/employees-list/employees-list.component';
import { PrlComponent } from './prl/prl.component';

@NgModule({
  declarations: [
    EmployeesListComponent,
    EmployeeComponent,
    EmployeesFiltersComponent,
    EmployeesComplexFiltersComponent,
    EmployeeMedicalDataComponent,
    PassportComponent,
    PassportWarningComponent,
    EmployeeAddTestModalComponent,
    RiskFactorsComponent,
    TestsComponent,
    MonitoringComponent,
    FeverRecordsComponent,
    EmployeePersonalDataComponent,
    EmployeeChangeStatusModalComponent,
    ResponsibleStatementComponent,
    PrlComponent,
    FeverModalComponent,
    ChangeStatusSimpleComponent
  ],
  providers: [EmployeeAddTestModalService, EmployeeService],
  imports: [
    CommonModule,
    HttpClientModule,
    I18NextModule,
    RouterModule,
    MatTabsModule,
    MatInputModule,
    MatIconModule,
    MatGridListModule,
    MatButtonModule,
    MatCheckboxModule,
    MatDividerModule,
    MatListModule,
    MatDatepickerModule,
    MatMomentDateModule,
    ReactiveFormsModule,
    MatButtonToggleModule,
    MatCardModule,
    UiModule,
    MatPaginatorModule,
    PipesModule,
    MatDialogModule,
    HttpClientModule,
    MatSnackBarModule,
    MatSelectModule
  ],
  exports: [EmployeesFiltersComponent, PassportComponent]
})
export class EmployeeModule {}
