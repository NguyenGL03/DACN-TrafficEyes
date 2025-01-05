import { Component, Input, ViewEncapsulation } from "@angular/core";
import { ActionWorkflow } from "@app/shared/core/controls/toolbar-workflow/toolbar-workflow.component";
import { CM_PROCESS_ENTITY } from "@shared/service-proxies/service-proxies";

@Component({
    selector: "auth-badge-workflow",
    templateUrl: './auth-badge-workflow.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AuthBadgeWorkflowComponent {
    @Input() authStatus: string;
    @Input() authStatusName: string;
    @Input() process: CM_PROCESS_ENTITY;
    action = ActionWorkflow;
}
