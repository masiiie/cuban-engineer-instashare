import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { FileService, InstaShareFile } from './file.service';
import { environment } from '../../environments/environment';

describe('FileService', () => {
  let service: FileService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.instaShareApiUrl}/api/files`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [FileService]
    });
    service = TestBed.inject(FileService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getFiles', () => {
    it('should return an array of files', () => {
      const mockFiles: InstaShareFile[] = [
        {
          id: 1,
          name: 'test.txt',
          status: 'Zipped',
          size: 1024,
          blobUrl: 'http://example.com/test.txt',
          created: new Date(),
          lastModified: new Date()
        }
      ];

      service.getFiles().subscribe(files => {
        expect(files).toEqual(mockFiles);
      });

      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockFiles);
    });
  });

  describe('uploadFile', () => {
    it('should upload a file and return file info', () => {
      const mockFile = new File(['test'], 'test.txt', { type: 'text/plain' });
      const mockResponse: InstaShareFile = {
        id: 1,
        name: 'test.txt',
        status: 'Uploading',
        size: 4,
        blobUrl: null,
        created: new Date(),
        lastModified: new Date()
      };

      service.uploadFile(mockFile).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${apiUrl}/upload`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body instanceof FormData).toBeTruthy();
      req.flush(mockResponse);
    });
  });

  describe('deleteFile', () => {
    it('should delete a file', () => {
      const fileId = 1;

      service.deleteFile(fileId).subscribe(response => {
        expect(response).toBeUndefined();
      });

      const req = httpMock.expectOne(`${apiUrl}/${fileId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });

  describe('downloadFile', () => {
    it('should download a file as blob', () => {
      const fileId = 1;
      const mockBlob = new Blob(['test content'], { type: 'text/plain' });

      service.downloadFile(fileId).subscribe(response => {
        expect(response).toEqual(mockBlob);
      });

      const req = httpMock.expectOne(`${apiUrl}/${fileId}/download`);
      expect(req.request.method).toBe('GET');
      req.flush(mockBlob);
    });
  });
});