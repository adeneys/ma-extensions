import { SingleItem } from '@sitecore/ma-core';

export class SetCustomFacetActivity extends SingleItem {
    getVisual(): string {
        const subTitle = this.isDefined ? this.params.parameters.rigel : '';
        const cssClass = this.isDefined ? '' : 'undefined';

        return `
            <div class="viewport-set-custom-facet marketing-action ${cssClass}">
                <span class="icon">
                    <img src="/~/icon/OfficeWhite/32x32/atom2.png" />
                </span>
                <p class="text with-subtitle" title="Set Custom Facet">
                    Set Custom Facet
                    <small class="subtitle" title="${subTitle}">${subTitle}</small>
                </p>
            </div>
        `;
    }

    get isDefined(): boolean {
        return Boolean(this.params.parameters.rigel != '');
    }
}
