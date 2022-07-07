// SPDX-License-Identifier: MIT
pragma solidity 0.8.4;

import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "./MostValuableRewardToken.sol";

contract MostValuableStakingContract is Ownable {
    ERC20 public StakingToken;
    MostValuableRewardToken public RewardToken;
    mapping(address => Stake) private stakers;

    struct Stake {
        uint256 stakedAtTimestamp;
        uint256 amount;
    }

    constructor(address _stakingTokenAddress, address _rewardTokenAddress) {
        StakingToken = ERC20(_stakingTokenAddress);
        RewardToken = MostValuableRewardToken(_rewardTokenAddress);
    }

    function stake(uint256 _amount) external {
        require(
            _amount > 0,
            "Amount not positive");

        require(
            StakingToken.allowance(msg.sender, address(this)) >= _amount,
            "Token transfer not approved");

        StakingToken.transferFrom(msg.sender, address(this), _amount);
        
        Stake memory stakeInfo = stakers[msg.sender];
        // check if already a staker - 
        if(stakeInfo.stakedAtTimestamp != 0) {
            claimRewards();
        }

        stakeInfo.stakedAtTimestamp = block.timestamp;
        stakeInfo.amount += _amount;
        stakers[msg.sender] = stakeInfo;
    }

    function withdraw() external {
        Stake memory stakeInfo = stakers[msg.sender];
        require(stakeInfo.amount > 0, 
            "Nothing staked for this address.");
        
        // transfer MVT
        StakingToken.transfer(msg.sender, stakeInfo.amount);
        claimRewards();
        delete stakers[msg.sender];
    }

    function claimRewards() public {
        uint256 rewards = calculateRewards(msg.sender);
        RewardToken.mint(rewards, msg.sender);
        stakers[msg.sender].stakedAtTimestamp = block.timestamp;
    }

    function calculateRewards(address _stakerAddress) public view returns(uint256) {
        Stake memory stakeInfo = stakers[_stakerAddress];
        uint256 rewardSize = (block.timestamp - stakeInfo.stakedAtTimestamp) * stakeInfo.amount;

        return rewardSize;
    }

    function checkStake(address _stakerAddress) public view returns (uint256){
        return stakers[_stakerAddress].amount;
    }
}