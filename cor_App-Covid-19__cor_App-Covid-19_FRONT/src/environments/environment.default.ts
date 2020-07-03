export const environment = {
  production: false,
  auth: {
    authority: 'https://foo-login.bar/tenant-UUID',
    clientID: '93c110d2-1749-476b-a8bf-0be567f5c914',
    redirectUri: 'http://localhost:4200',
    protectedResourceMap: [
      [
        '//localhost:4200/api',
        ['api://be833099-eac5-4f10-9fd1-879933e5579a/Read', 'api://be833099-eac5-4f10-9fd1-879933e5579a/Write']
      ],
      [
        '//localhost:4200/identity',
        ['api://be833099-eac5-4f10-9fd1-879933e5579a/Read', 'api://be833099-eac5-4f10-9fd1-879933e5579a/Write']
      ]
    ],
    scopes: ['api://be833099-eac5-4f10-9fd1-879933e5579a/Read', 'api://be833099-eac5-4f10-9fd1-879933e5579a/Write']
  },
  prefixes: {
    default: 'http://localhost:4200/api',
    identity: 'http://localhost:4200/identity'
  },
  host: '',
  feverThreshold: '37',
  url: {
    login: '/Admin/login/medicalServices',
    employees: {
      list: '/MedicalServices/employees/search',
      export: '/MedicalServices/employees/search/employees.csv',
      personal: '/MedicalServices/employee/{employeeId}',
      passport: '/MedicalServices/employee/{employeeId}/Passport',
      riskFactors: '/MedicalServices/employee/{employeeId}/riskfactors',
      responsibleStatement: '/MedicalServices/employee/{employeeId}/SymptomsInquiryResult',
      tests: '/MedicalServices/employee/{employeeId}/medicalTest',
      monitoring: '/MedicalServices/employee/{employeeId}/medicalMonitoring',
      addMonitoring: '/MedicalServices/employee/{employeeId}/monitoring',
      postQuickTest: '/MedicalServices/employee/{employeeId}/QuickTest',
      postPcrTest: '/MedicalServices/employee/{employeeId}/pcrTest',
      temperature: '/MedicalServices/employee/{employeeId}/TemperatureMedition',
      changePassport: '/MedicalServices/employee/{employeeId}/Passport',
      expirePassport: '/MedicalServices/employee/{employeeId}/Passport/expired',
      prlRedPassport: '/MedicalServices/employee/{employeeId}/Passport/prlred',
      hrRedPassport: '/MedicalServices/employee/{employeeId}/Passport/leavered',
      removeQuickTest: '/MedicalServices/employee/{employeeId}/medicalQuickTest/{testId}',
      removePcrTest: '/MedicalServices/employee/{employeeId}/medicalPcrTest/{testId}',
      removeMonitoring: '/MedicalServices/employee/{employeeId}/medicalMonitoring/{testId}'
    },
    alerts: {
      single: '/MedicalServices/alert/employee/{employeeId}',
      sendToAll: '/MedicalServices/alert/employees',
      sendToCountry: '/MedicalServices/alert/country',
      sendToDivision: '/MedicalServices/alert/division',
      sendToLocation: '/MedicalServices/alert/location'
    },
    masters: {
      monitoring: '/Master/MedicalMonitoring',
      countries: '/Master/countries',
      divisions: '/Master/divisions',
      statuses: '/Master/PassportStates',
      statusesHR: '/Master/PassportStates/RRHH',
      locations: '/Master/locations',
      regions: '/Master/regions',
      areas: '/Master/areas',
      cities: '/Master/cities',
      employees: '/Master/employees'
    },
    outsourcing: {
      create: '/Admin/employee',
      list: '/MedicalServices/externalEmployees/subordinates/search',
      export: '/MedicalServices/externalEmployees/subordinates/search/subordinates.csv',
      get: '/MedicalServices/externalEmployees/{employeeId}'
    },
    identity: {
      resetPassword: '/Users/reset'
    }
  }
};
