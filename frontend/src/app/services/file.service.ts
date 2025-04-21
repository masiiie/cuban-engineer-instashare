import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface InstaShareFile {
  id: number;
  name: string;
  status: 'OnlyInDbNoContent' | 'Uploading' | 'Zipping' | 'Zipped';
  size: number;
  blobUrl: string | null;
  created: Date;
  lastModified: Date;
}

@Injectable({
  providedIn: 'root'
})
export class FileService {
  private instaShareApiUrl = `${environment.instaShareApiUrl}/api/files`;

  constructor(private http: HttpClient) { }

  getFiles(): Observable<InstaShareFile[]> {
    return this.http.get<InstaShareFile[]>(this.instaShareApiUrl);
  }

  uploadFile(file: File): Observable<InstaShareFile> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<InstaShareFile>(`${this.instaShareApiUrl}/upload`, formData);
  }

  deleteFile(id: number): Observable<void> {
    return this.http.delete<void>(`${this.instaShareApiUrl}/${id}`);
  }

  downloadFile(id: number): Observable<Blob> {
    return this.http.get(`${this.instaShareApiUrl}/${id}/download`, { responseType: 'blob' });
  }
}