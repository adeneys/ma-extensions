import { Component, OnInit } from "@angular/core";
import { EditorBase } from "@sitecore/ma-core";

@Component({
    selector: 'set-custom-facet-editor',
    template: `
        <section class="content">
            <div class="form-group">
                <label for="rigel">Set the Rigel value</label>
                <input id="rigel" type="text" class="form-control" [(ngModel)]="rigel" />
            </div>
        </section>
    `
})

export class SetCustomFacetEditor extends EditorBase implements OnInit {
    rigel: string;

    ngOnInit(): void {
        this.rigel = this.model ? this.model.rigel || '' : '';
    }

    serialize(): any {
        return {
            rigel: this.rigel
        };
    }
}
