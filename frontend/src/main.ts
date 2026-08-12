import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { appConfig } from './app/app.config';
import { registerMedicalElements } from './app/web-components/register-elements';

bootstrapApplication(App, appConfig)
  .then((appRef) => registerMedicalElements(appRef.injector))
  .catch((err) => console.error(err));
