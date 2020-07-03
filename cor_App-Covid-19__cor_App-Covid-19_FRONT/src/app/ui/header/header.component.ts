import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { MatSelectChange } from '@angular/material/select';
import { I18NEXT_SERVICE, ITranslationService } from 'angular-i18next';
import { MastersService } from 'src/app/masters.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input()
  avatar: string;

  role: string;

  langs: { label: string; value: string }[] = [{ label: 'ESP', value: 'es' }, { label: 'ENG', value: 'en' }];

  lang = this.i18NextService.language;

  @Output()
  openNotifications: EventEmitter<void> = new EventEmitter();

  onChangeLang(event: MatSelectChange) {
    this.i18NextService.changeLanguage(event.value).then(val => {
      sessionStorage.setItem('i18nlang', event.value);
      this.lang = event.value;
      document.location.reload();
    });
  }

  constructor(private service: MastersService, @Inject(I18NEXT_SERVICE) private i18NextService: ITranslationService) {
    this.role = service.getRole();
  }

  ngOnInit(): void {}

  emitOpen() {
    this.openNotifications.emit();
  }
}
