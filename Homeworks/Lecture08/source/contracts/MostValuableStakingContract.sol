// SPDX-License-Identifier: MIT
pragma solidity 0.8.4;

import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/token/ERC20/ERC20.sol";

contract MostValuableStakingContract is Ownable {
    ERC20 public StakingToken;
    ERC20 public RewardToken;
    mapping(address => Stake) private stakers;

    struct Stake {
        uint256 stakedAtTimestamp;
        uint256 amount;
    }

    constructor(address _stakingTokenAddress, address _rewardTokenAddress) {
        StakingToken = ERC20(_stakingTokenAddress);
        RewardToken = ERC20(_rewardTokenAddress);
    }

    function stake(uint256 _amount) external {
        require(
            _amount > 0,
            "Amount not positive");

        require(
            StakingToken.allowance(msg.sender, address(this)) >= _amount,
            "Token transfer not approved");
        
        

        StakingToken.transferFrom(msg.sender, address(this), _amount);
        
        Stake memory stake = stakers[msg.sender];
        // check if already a staker - 
        if(stake.stakedAtTimestamp != 0) {
            
        }

        stake.stakedAtTimestamp = block.timestamp;
        stake.amount = _amount;
        stakers[msg.sender] = stake;
    }

    function withdraw() external {
        Stake memory stake = stakers[msg.sender];
        require(stake.amount > 0, 
            "Nothing staked for this address.");

        // claimRewards()
        // transfer MVT
        StakingToken.transfer(msg.sender, stake.amount);
        // remove stake
        delete stakers[msg.sender];
    }

    function checkStake(address _stakerAddress) public view returns (uint256){
        return stakers[_stakerAddress].amount;
    }
}