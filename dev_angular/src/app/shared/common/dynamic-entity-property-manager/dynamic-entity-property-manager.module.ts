import { NgModule } from '@angular/core';
 import { DynamicEntityPropertyManagerComponent } from './dynamic-entity-property-manager.component';
import { DynamicEntityPropertyValueModule } from '@app/admin/zero-base/dynamic-properties/dynamic-entity-properties/value/dynamic-entity-property-value.module';

@NgModule({
    declarations: [DynamicEntityPropertyManagerComponent],
    imports: [DynamicEntityPropertyValueModule],
    exports: [DynamicEntityPropertyValueModule, DynamicEntityPropertyManagerComponent],
})
export class DynamicEntityPropertyManagerModule {}
