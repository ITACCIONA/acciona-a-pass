import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { I18NextModule } from 'angular-i18next';
import { ConfigTestingModule, TranslateModule } from '../../commons';
import { EmployeeModule } from '../../employee/employee.module';
import { UiModule } from '../../ui/ui.module';
import { OutsourcingEmployeeComponent } from './outsourcing-employee.component';

describe('OutsourcingEmployeeComponent', () => {
  let component: OutsourcingEmployeeComponent;
  let fixture: ComponentFixture<OutsourcingEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [OutsourcingEmployeeComponent],
      imports: [
        TranslateModule,
        BrowserAnimationsModule,
        I18NextModule,
        MatIconModule,
        UiModule,
        MatTabsModule,
        MatCardModule,
        HttpClientModule,
        ConfigTestingModule,
        MatSnackBarModule,
        RouterTestingModule,
        EmployeeModule
      ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                id: '1'
              }
            }
          }
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutsourcingEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
