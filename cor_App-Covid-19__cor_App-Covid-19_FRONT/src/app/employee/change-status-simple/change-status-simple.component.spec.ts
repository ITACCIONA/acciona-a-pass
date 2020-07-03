import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { I18NextModule } from 'angular-i18next';
import { ConfigTestingModule, TranslateModule } from '../../commons';
import { UiModule } from '../../ui/ui.module';
import { EmployeeService } from '../employee.service';
import { ChangeStatusSimpleComponent } from './change-status-simple.component';

describe('ChangeStatusSimpleComponent', () => {
  let component: ChangeStatusSimpleComponent;
  let fixture: ComponentFixture<ChangeStatusSimpleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ChangeStatusSimpleComponent],
      imports: [
        ReactiveFormsModule,
        UiModule,
        TranslateModule,
        I18NextModule,
        MatDividerModule,
        MatDividerModule,
        MatButtonToggleModule,
        MatIconModule,
        HttpClientModule,
        ConfigTestingModule,
        MatSnackBarModule
      ],
      providers: [{ provide: MAT_DIALOG_DATA, useValue: {} }, { provide: MatDialogRef, useValue: {} }, EmployeeService]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeStatusSimpleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
