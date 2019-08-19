import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SetCustomFacetEditor } from './editor/set-custom-facet-editor.component';

@NgModule({
    imports:[
        FormsModule
    ],
    declarations: [SetCustomFacetEditor],
    entryComponents: [SetCustomFacetEditor]
})
export class SetCustomFacetModule { }