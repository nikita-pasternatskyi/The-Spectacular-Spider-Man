public class AirToGroundTransition : GroundedTransition
{
    protected override bool AdditionalCheck(bool result)
    {
        if(result == true)
        {
            return PlayerBody.Velocity.y <= 0;
        }

        return false;
    }
}
