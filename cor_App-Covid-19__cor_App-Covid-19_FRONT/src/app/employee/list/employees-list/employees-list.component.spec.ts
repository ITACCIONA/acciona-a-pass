import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { ConfigTestingModule, PipesModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from 'src/app/ui/ui.module';
import { MastersService } from '../../../masters.service';
import { EmployeeModule } from '../../employee.module';
import { EmployeesListComponent } from './employees-list.component';

class MockMasters {
  analytics = {
    parameters: []
  };
  getRole = () => {};
  roleName = ['PRL'];
  images = {
    parameters: [{ idParameter: '' }]
  };
}

describe('EmployeesListComponent', () => {
  let component: EmployeesListComponent;
  let fixture: ComponentFixture<EmployeesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        TranslateModule,
        I18NextModule,
        RouterTestingModule,
        MatIconModule,
        MatGridListModule,
        ReactiveFormsModule,
        MatInputModule,
        EmployeeModule,
        UiModule,
        MatTableModule,
        ConfigTestingModule,
        HttpClientModule,
        PipesModule
      ],
      providers: [{ provide: MastersService, useClass: MockMasters }]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
