import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { I18NextModule } from 'angular-i18next';
import { ConfigTestingModule, TranslateModule } from '../../commons';
import { EmployeeModule } from '../../employee/employee.module';
import { UiModule } from '../../ui/ui.module';
import { OutsourcingListComponent } from './outsourcing-list.component';

describe('OutsourcingListComponent', () => {
  let component: OutsourcingListComponent;
  let fixture: ComponentFixture<OutsourcingListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [OutsourcingListComponent],
      imports: [
        BrowserAnimationsModule,
        UiModule,
        EmployeeModule,
        MatCardModule,
        ConfigTestingModule,
        RouterTestingModule,
        TranslateModule,
        I18NextModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutsourcingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
