import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { I18NextModule } from 'angular-i18next';
import { of } from 'rxjs';
import { ConfigTestingModule, TranslateModule } from '../../commons';
import { MastersService } from '../../masters.service';
import { UiModule } from '../../ui/ui.module';
import { NewUserComponent } from './new-user.component';

class MasterMock {
  userInfo = { nombreUsuario: 'name' };
  fetchDivisions() {
    return of();
  }

  fetchLocations() {
    return of();
  }
}

describe('NewUserComponent', () => {
  let component: NewUserComponent;
  let fixture: ComponentFixture<NewUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewUserComponent],
      imports: [
        BrowserAnimationsModule,
        TranslateModule,
        I18NextModule,
        MatInputModule,
        MatSelectModule,
        MatAutocompleteModule,
        ReactiveFormsModule,
        MatCardModule,
        HttpClientModule,
        ConfigTestingModule,
        UiModule,
        MatSnackBarModule
      ],
      providers: [{ provide: MastersService, useClass: MasterMock }]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
