import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileService, InstaShareFile } from '../services/file.service';
import { Observable } from 'rxjs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { NavigationComponent } from '../shared/navigation/navigation.component';

@Component({
  selector: 'app-file-management',
  templateUrl: './file-management.component.html',
  styleUrls: ['./file-management.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule,
    NavigationComponent
  ]
})
export class FileManagementComponent implements OnInit {
  files$: Observable<InstaShareFile[]>;
  displayedColumns: string[] = ['name', 'status', 'size', 'created', 'actions'];
  
  constructor(
    private fileService: FileService,
    private snackBar: MatSnackBar
  ) {
    this.files$ = this.fileService.getFiles();
  }

  ngOnInit(): void {
    this.refreshFiles();
  }

  refreshFiles(): void {
    this.files$ = this.fileService.getFiles();
  }

  downloadFile(file: InstaShareFile): void {
    this.fileService.downloadFile(file.id).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = file.name;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        this.snackBar.open('Error downloading file', 'Close', { duration: 3000 });
        console.error('Download error:', error);
      }
    });
  }

  deleteFile(file: InstaShareFile): void {
    if (confirm(`Are you sure you want to delete ${file.name}?`)) {
      this.fileService.deleteFile(file.id).subscribe({
        next: () => {
          this.refreshFiles();
          this.snackBar.open('File deleted successfully', 'Close', { duration: 3000 });
        },
        error: (error) => {
          this.snackBar.open('Error deleting file', 'Close', { duration: 3000 });
          console.error('Delete error:', error);
        }
      });
    }
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
}