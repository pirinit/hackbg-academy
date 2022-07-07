// SPDX-License-Identifier: MIT
pragma solidity 0.8.4;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

contract MostValuableRewardToken is ERC20, Ownable {
    constructor() ERC20("Most Valuable Reward Token", "MVRT") {
    }

    function mint(uint256 _amount, address _recipient) external onlyOwner {
        _mint(_recipient, _amount);
    }
}