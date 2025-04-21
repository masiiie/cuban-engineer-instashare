import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileService } from '../services/file.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule
  ]
})
export class FileUploadComponent {
  @Output() fileUploaded = new EventEmitter<void>();
  isUploading = false;

  constructor(
    private fileService: FileService,
    private snackBar: MatSnackBar
  ) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0];
      this.uploadFile(file);
    }
  }

  private uploadFile(file: File): void {
    this.isUploading = true;
    this.fileService.uploadFile(file).subscribe({
      next: () => {
        this.isUploading = false;
        this.snackBar.open('File uploaded successfully', 'Close', { duration: 3000 });
        // Emit an event to notify parent component
        this.fileUploaded.emit();
      },
      error: (error) => {
        this.isUploading = false;
        this.snackBar.open('Error uploading file', 'Close', { duration: 3000 });
        console.error('Upload error:', error);
      }
    });
  }
}