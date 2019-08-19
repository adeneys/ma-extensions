import { Component, OnInit, Injector } from "@angular/core";
import { EditorBase } from "@sitecore/ma-core";

@Component({
    selector: 'set-custom-facet-editor',
    template: `
        <section class="content">
            <div class="form-group">
                <label>Set the Rigel value.</label>
            </div>
            <div class="form-group">
                <div class="row">
                    <input type="text" class="form-control" [(ngModel)]="rigel" />
                </div>
            </div>
        </section>
    `
})

export class SetCustomFacetEditor extends EditorBase implements OnInit {
    rigel: string;

    /*constructor(private injector: Injector) {
        super();
    }*/

    ngOnInit(): void {
        this.rigel = this.model ? this.model.rigel || '' : '';
    }

    serialize(): any {
        return {
            rigel: this.rigel
        };
    }
}
