import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from "@angular/forms";
import { AutoComplete } from "primeng/primeng";

import { ConfigurationService, UserService } from "../../../../../shared/services";
import { User } from "../../../../../shared/models";

@Component({
  selector: 'app-new-form-general',
  templateUrl: './new-form-general.component.html',
  styleUrls: ['./new-form-general.component.scss']
})
export class NewFormGeneralComponent implements OnInit {
  @ViewChild(AutoComplete)
  autoCompleteComponent: AutoComplete;
  @Input() generalFormGroup: FormGroup;
  public managerValid: boolean = false;
  public productManagerSearchResults: User[];
  public currentUser: User;

  constructor(private configurationService: ConfigurationService, private userService: UserService) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
    // Perform custom validation on the manager FormControl
    this.validateManager(this.generalFormGroup.value);
    this.generalFormGroup.controls.Manager.valueChanges.subscribe(manager => {
      this.validateManager(manager);
    });
  }

  public searchProductManagers(query: string): void {
    this.configurationService.getProductManagers().subscribe(productManagers => {
      this.productManagerSearchResults = productManagers.filter(productManager => {
        return productManager.displayName.toLowerCase().includes(query.toLowerCase());
      });
    });
  }

  public onDropdownSelect() {
    if (this.autoCompleteComponent.panelVisible) this.autoCompleteComponent.hide();
  }

  /**
   * Validating the manager FormControl wasn't good enough for satisfying the required nature of the field
   * Since we're really looking for the manager objectSid, we can check for the presence of that property
   * @param value value of the generalFormGroup
   */
  private validateManager(manager) {
    if (manager) {
      this.managerValid = manager.objectSid !== undefined;
    } else {
      this.managerValid = false;
    }
  }

}
