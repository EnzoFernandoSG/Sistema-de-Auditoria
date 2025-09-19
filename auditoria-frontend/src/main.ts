import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { App } from './app/app';
import { routes } from './app/app.routes';
import { provideIcons } from '@ng-icons/core';
import { provideHttpClient, withFetch } from '@angular/common/http'; // <--- Adicione 'withFetch' aqui

import {
  lucideArrowLeft,
  lucideSend,
  lucideTriangleAlert,
  lucideHouse,
  lucideChartBar,
  lucideCircleCheck,
  lucideLoader,
  lucideDatabase,
  lucideWifi,
  lucideWifiOff,
  lucideRefreshCcw,
  lucideBuilding2,
  lucideExternalLink,
  lucideCalendar,
  lucideFileText,
  lucideLayers,
  lucideShield,
  lucideUser,
  lucideDatabaseZap,
  lucideRuler,
  lucideTrash2,
  lucideMessageSquare,
  lucideConstruction,
  lucideUserCheck,
  lucideImage,
  lucideX,
  lucideCalendarDays,
  lucideHistory,
  lucideZap,
  lucideAward,
  lucideClipboardCheck,
  lucidePlane,
  lucideHotel,
  lucideGavel,
  lucideSearch
} from '@ng-icons/lucide';

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),
    provideHttpClient(withFetch()), // <--- Chame withFetch() dentro de provideHttpClient()
    provideIcons({
      lucideArrowLeft, lucideSend, lucideTriangleAlert, lucideHouse, lucideChartBar, lucideCircleCheck, lucideLoader,
      lucideDatabase, lucideWifi, lucideWifiOff, lucideRefreshCcw, lucideBuilding2, lucideExternalLink, lucideCalendar,
      lucideFileText, lucideLayers, lucideShield, lucideUser, lucideDatabaseZap, lucideRuler, lucideTrash2,
      lucideMessageSquare, lucideConstruction, lucideUserCheck, lucideImage, lucideX, lucideCalendarDays,
      lucideHistory, lucideZap, lucideAward, lucideClipboardCheck, lucidePlane, lucideHotel, lucideGavel, lucideSearch
    })
  ]
});