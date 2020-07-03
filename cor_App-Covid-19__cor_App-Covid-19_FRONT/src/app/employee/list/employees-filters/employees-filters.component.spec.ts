import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ConfigTestingModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { EmployeesFiltersComponent } from './employees-filters.component';

describe('EmployeesFiltersComponent', () => {
  let component: EmployeesFiltersComponent;
  let fixture: ComponentFixture<EmployeesFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EmployeesFiltersComponent],
      imports: [
        TranslateModule,
        I18NextModule,
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatGridListModule,
        MatInputModule,
        MatIconModule,
        HttpClientModule,
        ConfigTestingModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeesFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
