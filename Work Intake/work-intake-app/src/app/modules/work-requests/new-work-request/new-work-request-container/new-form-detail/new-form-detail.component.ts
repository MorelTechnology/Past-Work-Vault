import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from "@angular/forms";
import { SelectItem } from 'primeng/components/common/selectitem';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap/tooltip/tooltip';

@Component({
  selector: 'app-new-form-detail',
  templateUrl: './new-form-detail.component.html',
  styleUrls: ['./new-form-detail.component.scss']
})
export class NewFormDetailComponent implements OnInit {
  @Input() detailFormGroup: FormGroup;
  public businessValueUnits: SelectItem[];
  public impactValues: SelectItem[];
  public showDetailHelp: boolean = false;

  constructor() {
    this.businessValueUnits = [
      { value: 1, label: 'Dollars per year' },
      { value: 2, label: 'Hours per year' }
    ];
    this.impactValues = [ 
      { value: 1, label: 'Very High' },
      { value: 2, label: 'High' },
      { value: 3, label: 'Low' },
      { value: 4, label: 'Very Low' }
    ];
  }

  ngOnInit() {
  }

  public showTooltip(tooltip: NgbTooltip): void {
    tooltip.placement = window.innerWidth <= 992 ? "top": "left";
    tooltip.open();
  }

  public dismissTooltip(tooltip: NgbTooltip): void {
    setTimeout(() => {
      tooltip.close();
    }, 250);
  }

}
