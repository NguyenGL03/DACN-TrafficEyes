import { NgModule,Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // Import CommonModule
import { UploadImagesComponent } from './uploadimages.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CommonDeclarationDeclarationModule } from '@app/shared/core/utils/CommonDeclarationModule';



@NgModule({
  declarations: [UploadImagesComponent],
  imports: [
    CommonModule, // Include CommonModule here
    ReactiveFormsModule, // Required for Reactive Forms
    FormsModule,
    HttpClientModule,
    CommonDeclarationDeclarationModule
  ],
  providers: [provideNativeDateAdapter()],
  exports: [UploadImagesComponent],
})
export class UploadImagesModule {}
