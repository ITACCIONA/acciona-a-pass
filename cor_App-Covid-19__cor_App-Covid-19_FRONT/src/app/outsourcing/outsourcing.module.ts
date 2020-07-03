import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { RouterModule } from '@angular/router';
import { I18NextModule } from 'angular-i18next';
import { TranslateModule } from '../commons';
import { EmployeeModule } from '../employee/employee.module';
import { UiModule } from '../ui/ui.module';
import { NewUserComponent } from './new-user/new-user.component';
import { OutsourcingEmployeeComponent } from './outsourcing-employee/outsourcing-employee.component';
import { OutsourcingListComponent } from './outsourcing-list/outsourcing-list.component';

@NgModule({
  declarations: [OutsourcingListComponent, OutsourcingEmployeeComponent, NewUserComponent],
  imports: [
    CommonModule,
    MatCardModule,
    TranslateModule,
    I18NextModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatAutocompleteModule,
    UiModule,
    MatTabsModule,
    MatIconModule,
    MatDialogModule,
    EmployeeModule,
    RouterModule
  ]
})
export class OutsourcingModule {}
