import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { Observable, Subject, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';


@Injectable({providedIn : 'root'})
export class ApiService {
  options = {};
  base_url = environment.api;

  constructor(private http: HttpClient) {
    var httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    this.options = httpHeaders;
  }

  public post(url: string, obj: any): Observable<any> {
    return this.http.post(this.base_url + url, obj, this.options).pipe(
      map((res: string) =>      
      JSON.parse(res)      
      ),
      catchError(this.handleError),
    )
  }

  public postPromise(url: string, obj: any) {
 
    return this.http.post(this.base_url + url, obj, this.options).pipe(
      map((res: string) =>
        JSON.parse(res)
      ),
      catchError(this.handleError),
    ).toPromise()
  }

  private handleError(error: any) {      
    console.log('Handling error locally and rethrowing it...', error);
    return throwError(error);
  }
}
