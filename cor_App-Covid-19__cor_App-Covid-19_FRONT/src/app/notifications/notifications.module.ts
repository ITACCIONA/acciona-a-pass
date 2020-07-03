import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../ui/ui.module';
import { NotificationsComponent } from './notifications/notifications.component';
import { SendNotificationGeneralComponent } from './send-notification-general/send-notification-general.component';
import { SendNotificationSingleComponent } from './send-notification-single/send-notification-single.component';
import { GeneralFormComponent } from './general-form/general-form.component';

@NgModule({
  declarations: [SendNotificationGeneralComponent, SendNotificationSingleComponent, NotificationsComponent, GeneralFormComponent],
  imports: [
    CommonModule,
    I18NextModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatInputModule,
    UiModule,
    MatButtonToggleModule,
    MatIconModule,
    MatDialogModule,
    MatAutocompleteModule,
    MatSlideToggleModule,
    MatCardModule
  ]
})
export class NotificationsModule {}
