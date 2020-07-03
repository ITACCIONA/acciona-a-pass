import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ConfigService } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../../ui/ui.module';
import { EmployeeAddTestModalComponent } from '../employee-add-test-modal/employee-add-test-modal.component';
import { EmployeeAddTestModalService } from '../employee-add-test-modal/employee-add-test-modal.service';

describe('EmployeeAddTestModalComponent', () => {
  let component: EmployeeAddTestModalComponent;
  let fixture: ComponentFixture<EmployeeAddTestModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        I18NextModule.forRoot(),
        MatDividerModule,
        MatFormFieldModule,
        MatDatepickerModule,
        MatButtonToggleModule,
        MatDialogModule,
        MatNativeDateModule,
        ReactiveFormsModule,
        MatIconModule,
        MatInputModule,
        NoopAnimationsModule,
        HttpClientModule,
        MatSnackBarModule,
        UiModule
      ],
      providers: [
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: [] },
        EmployeeAddTestModalService,
        ConfigService
      ],
      declarations: [EmployeeAddTestModalComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeAddTestModalComponent);
    component = fixture.componentInstance;
    component.data = { tests: { field: ['asd'] }, employeeId: 1 };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
