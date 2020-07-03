import { APP_INITIALIZER, LOCALE_ID } from '@angular/core';
import { I18NEXT_SERVICE, ITranslationService } from 'angular-i18next';

export function appInit(i18next: ITranslationService) {
  return () =>
    i18next.init({
      whitelist: ['en', 'es'],
      fallbackLng: 'es',
      lng: i18next.language || sessionStorage.getItem('i18nlang') || 'es',
      debug: false,
      returnEmptyString: false,
      ns: ['translation', 'validation', 'error'],
      resources: {
        es: {
          translation: require('../../../../i18n/es.json')
        },
        en: {
          translation: require('../../../../i18n/en.json')
        }
      }
    });
}

export function localeIdFactory(i18next: ITranslationService) {
  return i18next.language;
}

export const I18N_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: appInit,
    deps: [I18NEXT_SERVICE],
    multi: true
  },
  {
    provide: LOCALE_ID,
    deps: [I18NEXT_SERVICE],
    useFactory: localeIdFactory
  }
];
