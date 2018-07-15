import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class BaseApiService {

  constructor(private http: HttpClient) { }

  public get(url: string) {
    return this.http.get(url, { observe: 'response' });
  }

  public post(url: string, body?: any) {
    return this.http.post(url, body, { observe: 'response' });
  }

  public put(url: string, body: any) {
    return this.http.put(url, body, { observe: 'response' });
  }

  public delete(url: string) {
    return this.http.delete(url, { observe: 'response' });
  }

}
