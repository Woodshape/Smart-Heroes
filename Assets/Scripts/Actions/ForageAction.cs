namespace Actions {
    public class ForageAction : Action {
        
        public override bool CanRun() {
            return true;
        }
        
        public override int CalculateCost() {
            return 1;
        }
    }
}
