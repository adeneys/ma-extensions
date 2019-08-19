import { Plugin } from '@sitecore/ma-core';
import { SetCustomFacetActivity } from './set-custom-facet-activity';
import { SetCustomFacetEditor } from '../codegen/editor/set-custom-facet-editor.component';
import { SetCustomFacetModuleNgFactory } from '../codegen/set-custom-facet-module.ngfactory';

@Plugin({
    activityDefinitions: [
        {
            id: '5862E864-6CCF-4673-84BE-4D5C59BADE8F',
            activity: SetCustomFacetActivity,
            editorComponenet: SetCustomFacetEditor,
            editorModuleFactory: SetCustomFacetModuleNgFactory
        }
    ]
})
export default class MAExtensionsPlugin {}
