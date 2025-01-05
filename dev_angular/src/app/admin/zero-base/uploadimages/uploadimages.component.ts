import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpResponse,HttpClient,HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FileUploadService } from './file-upload.service';
import { FormsModule } from '@angular/forms';
import { ImageServiceProxy, FileParameter } from '@shared/service-proxies/service-proxies';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {provideNativeDateAdapter} from '@angular/material/core';



@Component({
  selector: 'app-image-upload',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,],
  templateUrl: './uploadimages.component.html',
  styleUrls: ['./uploadimages.component.css','./theme.scss'],
  providers: [provideNativeDateAdapter()],
})
export class UploadImagesComponent implements OnInit {
  imageSrc: string | ArrayBuffer | null = null;
  imageName: string | null = null;
  currentFile?: File;
  message = '';
  preview = '';
  imgDetails = {
    location: '',
    date: '',
  };
  minDate: string; // Minimum date (1st Jan 2000)
  maxDate: string;
  imageInfos?: Observable<any>;
 selectedDate: string| null = null;
 isLoading: boolean = false;
 resultData: { cars: number; motorcycles: number } | null = null;

  constructor(private uploadService: FileUploadService, private http: HttpClient, private _imageService: ImageServiceProxy) { 
    const today = new Date();
    this.minDate = '2000-01-01'; // Fixed minimum date
    this.maxDate = today.toISOString().split('T')[0]; 
  }

  async submitForm(form: any): Promise<void> {
    console.log(this.selectedDate);
    this.isLoading = true; // Bật trạng thái loading
    if (form.valid) {
      const [year, month, day] = this.selectedDate.split('-');
      this.imgDetails.date = `${day}-${month.padStart(2, '0')}-${year}`;
      const fileName = this.imgDetails.location + '_' + this.imgDetails.date+ '.' + this.currentFile.name.split('.').pop();
      const fileParameter: FileParameter = {
        data: this.currentFile,
        fileName:fileName
      }
      console.log(fileName);
      this._imageService.uploadImage(fileParameter).subscribe(res => {
        this.resultData = res;
        this.isLoading = false; // Tắt trạng thái loading
        console.log('result data: ', res);
      },
      (err) => {
        console.error(err);
        this.isLoading = false; // Tắt trạng thái loading nếu có lỗi
      })
    }
  };


  ngOnInit(): void {
    this.imageInfos = this.uploadService.getFiles();
  }

  onDateChange(event: Event): void{
    const input = event.target as HTMLInputElement;
    console.log(input);
  }

  onFileSelect(event: Event): void {
     const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];

      if (file.type.startsWith('image/')) {
        this.imageName = file.name; // Save file name
        const reader = new FileReader();
        reader.onload = () => {
          this.imageSrc = reader.result; // Save image source for preview
        };
        this.currentFile = file;
        reader.readAsDataURL(file); // Read the file as a data URL
      } else {
        alert('Invalid file type. Please select an image.');
        input.value = ''; // Clear invalid file input
      }
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault(); // Prevent default behavior (e.g., open file in new tab)
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      const file = event.dataTransfer.files[0];

      if (file.type.startsWith('image/')) {
        this.imageName = file.name; // Save file name
        const reader = new FileReader();
        reader.onload = () => {
          this.imageSrc = reader.result; // Save image source for preview
        };
        reader.readAsDataURL(file); // Read the file as a data URL
      } else {
        alert('Invalid file type. Please drop an image.');
      }
    }
  }
  private convertToBase64(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);

    reader.onload = () => {
      if (reader.result) {
        // Ensure the Base64 string has the correct data URL format
        const base64String = reader.result.toString();
        
        // Check if it's a valid image and add the appropriate prefix
        if (base64String.startsWith('data:image')) {
          resolve(base64String); // Return the correctly formatted Base64 string
        } else {
          reject('Invalid image format in Base64 string');
        }
      } else {
        reject('FileReader returned null or undefined');
      }
    };

    reader.onerror = (error) => reject(error);
  });
}

}