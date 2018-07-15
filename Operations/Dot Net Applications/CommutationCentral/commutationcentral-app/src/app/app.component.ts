import { Component, AfterContentInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterContentInit {
  title = 'app';

  ngAfterContentInit(): void {
    // Add an app-wide event listener for the backspace key in a read-only field and suppress the event. Internet Explorer was detecting such an event as a navigation backwards
    window.addEventListener('keydown', (e: any) => {
      if (e.which === 8 && e.target.readOnly) {
        e.preventDefault();
      }
    });
  }
}
