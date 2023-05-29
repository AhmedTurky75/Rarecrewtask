import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { AttendanceDataModule } from './attendance-data/attendance-data.module';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AttendanceDataModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
