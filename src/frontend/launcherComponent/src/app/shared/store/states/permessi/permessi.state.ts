import { Selector, State } from '@ngxs/store';
import { PermissionFeatures } from '../../../enum/permission-features.enum';
import { Role } from '../../../model/utente.model';
import { PermessiFeatureInterface } from '../../../interface/permessi-feature.interface';

export interface PermessiStateModel {
    permessi: Array<PermessiFeatureInterface>;
}

export const PermessiStateDefaults: PermessiStateModel = {
    permessi: [
        {
            feature: PermissionFeatures.SchedaTelefonata,
            roles: [Role.GestoreChiamate]
        }
    ]
};

@State<PermessiStateModel>({
    name: 'permessi',
    defaults: PermessiStateDefaults
})
export class PermessiState {

    @Selector()
    static permessi(state: PermessiStateModel) {
        return state.permessi;
    }

}