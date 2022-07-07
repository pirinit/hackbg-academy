// SPDX-License-Identifier: MIT
pragma solidity 0.8.4;

import "@openzeppelin/contracts/access/Ownable.sol";

contract MostValuableStakingContract is Ownable {
    address public StakingTokenAddress;
    address public RewardTokenAddress;

    constructor(address _stakingTokenAddress, address _rewardTokenAddress) {
        StakingTokenAddress = _stakingTokenAddress;
        RewardTokenAddress = _rewardTokenAddress;
    }
}