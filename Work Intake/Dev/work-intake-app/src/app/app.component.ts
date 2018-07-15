import { Component, AfterContentInit, OnInit } from '@angular/core';
import { environment } from "../environments/environment";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterContentInit, OnInit {
  title = 'app';

  ngAfterContentInit(): void {
    // Add an app-wide event listener for the backspace key in a read-only field and suppress the event. Internet Explorer was detecting such an event as a navigation backwards
    window.addEventListener('keydown', (e: any) => {
      if (e.which === 8 && e.target.readOnly) {
        e.preventDefault();
      }
    });
  }

  ngOnInit() {
    if (location.protocol === 'http:' && environment.production) {
      let redirectUri = location.href.replace('http', 'https').replace(location.hostname, `${location.hostname}.trg.com`).replace(location.port, `${environment.securePort}`);

      window.location.href = redirectUri;
    }
  }
}
