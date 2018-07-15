import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-button',
  templateUrl: './nav-button.component.html',
  styleUrls: ['./nav-button.component.scss']
})
export class NavButtonComponent implements OnInit {
  @Input() public icon: string;
  @Input() public displayText: string;
  @Input() public route: string;
  @Input() public routeParameters: string;

  constructor(public router: Router) { }

  ngOnInit() {
  }

  public navigate(): void {
    let destination = [this.route];
    if (this.routeParameters) destination.push(this.routeParameters);
    this.router.navigate(destination);
  }

}
